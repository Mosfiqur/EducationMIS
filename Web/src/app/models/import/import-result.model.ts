import { IRowError } from './row-error';

export interface IImportResult {
    invalidFile: boolean;
    totalImported: number;
    importedObjects: any[];
    isSuccess: boolean;
    errorMessage: string;
    rowErrors: IRowError[];
}