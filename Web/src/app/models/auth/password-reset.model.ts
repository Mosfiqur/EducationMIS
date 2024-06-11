export interface IPasswordResetModel {
    newPassword: string;
    confirmPassword: string;
    token: string;
}