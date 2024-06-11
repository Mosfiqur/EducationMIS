export interface IPagedResponse<T> {
    data: Array<T>;
    pageSize: number,
    pageNo: number,
    total: number,
}