export interface ILoginResponse {
    token: string;
    statusCode: string;
    message: string;
    userProfile: IUserProfile
}

export interface IUserProfile{
    id:number; 
    bearerToken: string;	
	fullName: string;
	levelName: string;
	roleName: string;
	email: string;
    phoneNumber: string;
    designationName:string
}