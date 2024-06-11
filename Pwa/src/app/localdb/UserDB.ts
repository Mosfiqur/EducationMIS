import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { Observable } from 'rxjs';
import { resolve } from 'url';
import { User } from '../models/idbmodels/indexedDBModels';
import { UnicefDBSchema } from './UnicefDBSchema';

@Injectable({
    providedIn: 'root'
}) 
/**
 * A service which contains all user apis
 */
export class UserDB {
    constructor(private dbService: NgxIndexedDBService) {

    }

    saveUser(user: User): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this.dbService.add(UnicefDBSchema.TableNames.tbl_user, user)
                .subscribe((key) => {
                    console.log('Key = ', key);
                    resolve(true);
                }, err => {
                    console.log('Error: ', err);
                    reject(false);
                });
        });
    }

    deleteUser(): Observable<boolean> { 
        return this.dbService.clear(UnicefDBSchema.TableNames.tbl_user);
    }

    getUser() {  
        return this.dbService.getAll(UnicefDBSchema.TableNames.tbl_user);
    } 
}