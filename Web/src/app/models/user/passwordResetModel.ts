export interface IPasswordResetModel {
    userId: number;
    newPassword: string;
}

export interface IPasswordChangeModel {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export interface IUserPasswordResetModel {
    newPassword: string;
    confirmPassword: string;
}