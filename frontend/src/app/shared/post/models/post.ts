import { Author } from "./author";

export interface Post{
    id: string;
    author: Author;
    description: string;
    multimediaUrls: string[];
    tags: string[];
    createdAt: Date;
    likesCount: number;
    likedByUser: boolean;
    commentsCount: number;
}