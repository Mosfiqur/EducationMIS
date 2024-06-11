export interface ILoginResponse {
    permissions: string[];
    token: string;
    statusCode: string;
    message: string;
    userProfile: IUserProfile
}

export interface IUserProfile{
    bearerToken: string;	
	fullName: string;
    levelName: string;
    roleId:number;
	roleName: string;
	email: string;
    phoneNumber: string;
    id:number;
    designationName:string;
}