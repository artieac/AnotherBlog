export interface IUser {
    Id: number;
    UserName: string;
    Email: string;
    FirstName: string;
    LastName: string;
    DisplayName: string;
    ApprovedCommenter: boolean;
    IsSiteAdministrator: boolean;
    About: string;
    Roles: { [key: number]: number };
}
