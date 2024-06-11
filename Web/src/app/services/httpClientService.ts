import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { HttpErrorResponse, HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class HttpClientService {

    constructor(private http: Http,
        private toastrService: ToastrService,
        private httpClient: HttpClient
    ) { }


    postAsync(api: string, body: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.httpClient.post(api, body).subscribe({ next: resolve, error: reject });
        });
    }


    putAsync(api: string, body: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.httpClient.put(api, body).subscribe({ next: resolve, error: reject });
        });
    }


    getAsync(api: string): Promise<any> {
        return new Promise((resolve, reject) => {
            this.httpClient.get(api).subscribe({ next: resolve, error: reject });
        });
    }

    deleteAsync(api: string): Promise<any> {
        return new Promise((resolve, reject) => {
            this.httpClient.delete(api).subscribe({ next: resolve, error: reject });
        });
    }

    downloadAsync(api: string, body: any) {
        return new Promise((resolve, reject) => {
            this.httpClient.post(api, body, {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                })
                , responseType: 'blob'
            }).subscribe({ next: resolve, error: reject });
        });
    }

    download(api: string, body: any) : Promise<Blob>{
        return new Promise((resolve, reject) => {
            this.httpClient.post<Blob>(api, body, {responseType: 'blob' as 'json', observe: 'response'})
            .pipe(
                map(x => {   
                    let blob = x.body;                    
                    var contentDisposition = x.headers.get('Content-Disposition');
                    var filename = contentDisposition.split(';')[1].split('filename')[1].split('=')[1].trim();                    

                    if (window.navigator && window.navigator.msSaveOrOpenBlob) { // for IE
                        window.navigator.msSaveBlob(blob, filename);
                      } else {
                        var objectUrl = window.URL.createObjectURL(blob);
                        var link = document.createElement('a');
                        link.setAttribute('download', filename);
                        link.setAttribute('href', objectUrl);
                        link.click();
                      }
                    return x.body
                })
            )            
            .subscribe({ next: resolve, error: reject });
        });
    }
    // postAsync(api: string, body: any): Promise<any> {
    //     return new Promise((resolve, reject) => {
    //         this.http.post(api, body, this.getToken()).pipe(
    //             map((res: Response) => {
    //                 // temporary fix to avoid exception in persing empty body
    //                 // need to fix
    //                 try
    //                 {
    //                     return res.json()
    //                 }
    //                 catch{

    //                 }
    //             }))
    //             .subscribe((data) => {
    //                 resolve(data);
    //             }, (err) => {                       
    //                 this.handleError(err);
    //                 reject(err);
    //             });
    //     });
    // }

    // putAsync(api: string, body: any): Promise<any> {
    //     return new Promise((resolve, reject) => {
    //         this.http.put(api, body, this.getToken()).pipe(
    //             map((res: Response) => {
    //                 // temporary fix to avoid exception in persing empty body
    //                 // need to fix
    //                 try
    //                 {
    //                     return res.json()
    //                 }
    //                 catch{

    //                 }
    //             }))
    //             .subscribe((data) => {                    
    //                 resolve(data);
    //             }, (err) => {                   
    //                 this.handleError(err);
    //                 reject(err);
    //             });
    //     });
    // }

    // getAsync(api: string): Promise<any> {
    //     return new Promise((resolve, reject) => {
    //         this.http.get(api, this.getToken()).pipe(
    //             map((res: Response) => res.json()))
    //             .subscribe((data) => {
    //                 resolve(data);
    //             }, (err) => {
    //                 this.handleError(err);
    //                 reject(err);
    //             });
    //     });
    // }

    // deleteAsync(api: string): Promise<any> {
    //     return new Promise((resolve, reject) => {
    //         this.http.delete(api, this.getToken())
    //             .subscribe((data) => {                    
    //                 resolve(data);
    //             }, (err) => {
    //                 this.handleError(err);
    //                 reject(err);
    //             });
    //     });
    // }

    //Private Methods
    private getToken(): any {
        // create authorization header with jwt token
        let token = localStorage.getItem('bearerToken');
        if (token) {
            let headers = new Headers({ 'Authorization': "Bearer " + token });
            return new RequestOptions({ headers: headers });
        }
        else {
            let headers = new Headers({ 'Content-Type': 'application/json' });
            return new RequestOptions({ headers: headers });
        }

    }

    handleError(err: any) {
        let errorMsg = "";
        let defaultMsg: "An unexpected error ocured."
        if (err.error instanceof ErrorEvent) {
            errorMsg = `${err.error.Error}`;
        }
        // server side error
        else {
            if (err.status === 500) {
                errorMsg = defaultMsg;
            }
            else {
                errorMsg = `${err.json().Error}`;
            }
        }        
        this.toastrService.error(errorMsg || defaultMsg);
    }
}
