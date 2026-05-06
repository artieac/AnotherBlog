import { IBlog } from './IBlog';
import { IUser } from './IUser';
import { ITag } from './ITag';

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
    Tags: ITag[];
}
