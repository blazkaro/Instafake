import { PostAuthor } from "./post-author";

export interface Post{
    id: string;
    author: PostAuthor;
    description: string;
    multimediaUrls: string[];
    tags: string[];
    createdAt: Date;
    likesCount: number;
    likedByUser: boolean;
    commentsCount: number;
}