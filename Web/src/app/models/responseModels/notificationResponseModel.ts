export interface INotificationResponse {
    data: Array<Notification>;
    pageSize: number;
    pageNo: number;
    total: number;
    notActedTotal:number;
}