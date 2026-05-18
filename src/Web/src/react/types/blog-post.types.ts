import { IBlog } from './blog.types';
import { IUser } from './user.types';

export interface IBlogPost {
    Id: number;
    IsPublished: boolean;
    Blog?: IBlog;
    Author?: IUser;
    EntryText: string;
    Title: string;
    DatePosted: string;
    DateCreated: string;
    CommentCount: number;
    TimesViewed: number;
}

