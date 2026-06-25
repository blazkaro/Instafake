import { Author } from "./author";

export interface PostComment{
    id: string;
    author: Author;
    content: string;
    createdAt: Date;
}