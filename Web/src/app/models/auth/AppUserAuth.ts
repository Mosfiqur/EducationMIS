export class AppUserAuth {
    bearerToken: string = "";
	isAuthenticated: boolean = false;
	id:number;
	fullName: string = "";
	levelName: string = "";
	roleId: number;
	roleName: string = "";
	email: string = "";
	phoneNumber: string= "";
  	permissions: string[];
}