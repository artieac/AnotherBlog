export interface IBlogListItem {
    Id: number;
    BlogListId: number;
    Name: string;
    RelatedLink: string;
    DisplayOrder: number;
}

export interface IBlogList {
    Id: number;
    BlogId: number;
    Name: string;
    ShowOrdered: boolean;
    Items: IBlogListItem[];
}
