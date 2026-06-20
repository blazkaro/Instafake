# Instafake
App that tries to behave (not necessarily look) like Instagram. Built with high-scale kept in mind.

## Current local dev architecture
![Architecture](./local_dev_architecture.svg)

## Important or not obvious decisions
### BFF (Backend for Frontend)
- #### Why this pattern?
    - In order to provide best users security, OAuth2 confidential client must be used. That client cannot be frontend app. BFF can achieve that via using basic cookie auth, where cookies depend on OIDC auth process. Furthermore, BFF can also act as API Gateway, aggregator and reverse proxy. Switches between authz forms, seamlessly handles access token expiration.

### Multimedia service
- #### Why AWS S3 compatible storage?
    - AWS S3 provides pre-signed upload URLs feature. It allows to securely upload multimedia directly to the storage, effectivelly omitting proxying through internal services thus saving a lot of resources.

### Identity events ingress service
- #### Why does it require Kafka idempotence (thus acks=all is forced)?
    - We avoid duplicated events from producer retries. More importantly, identity events are the most important ones in whole workflow. If event is not handled successfully, users may have problems with performing actions in the future (e.g. handle of 'user.created' failed, user cannot create posts etc.).

### Posts service
- #### Why clean architecture?
    - In this project, domain is not the source of complexity. It may look like overengineering, especially when we build app that has to work on high-scale. However, the expected amount of integrations and the need to emphasize specific aspects of the code makes it a great choice.
- #### What are these specific aspects?
    - Performance. In this project CQRS implementation breaks some DDD rules - and it's done purposely. Typically in DDD, command/query handlers sit in the application layer, but this project implements query handlers in infrastructure layer. It screams loudly: "that handler has to sit close to the database and make use of all optimization techniques this database provides". The use of loosely coupled anemic domain entities (though they are intuitively very coupled) comes up with similar effect. We seek performance, not ideal consistency.

    - Of course, we could create more specialized application layer repositories or services in order to satisfy DDD. However, that would create empty, pass-through abstraction layers and a lot of unnecessary code while losing the emphasis of what is really important.


- #### Why CQRS, and how?
    - Again, performance. We don't want to load whole infrastructure layer entities (a lot of data, joins, relationships), map to domain layer entities, then to application layer DTOs. We just want some data to satisfy UI! Additionally, the demand of QUERY posts is expected to be much higher than CREATE/LIKE post. In the future, the clear separation of read/write could be helpful when redesigning db architecture (e.g. separate write-only and read-only).
    - As mentioned above, query handlers sit in infrastructure layer. But what about commands? They are in application layer, because during writes we want our data to be valid (in application layer we are forced to use repositories, which use domain entities, which always preserve valid state)

- #### A bit about database
    - Some entities are denormalized. Why? You already know :). For example, reading count of items from row is much faster than performing COUNT().
    - Composite indexes on {date, id} (id is tie breaker). They are used for very efficient cursor pagination - we can use WHERE on these fields, and pagination is done in O(logN + K), without growing linearly like with offset. Without this composite index, the cursor pagination would still need to perform O(n) search.