import { AbstractControl, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ValidationMessage } from '../utility/ValidationMessage';


export class ValidationErrorBuilder{

    private itemsToSkip: string[];
    private validationMessages: {[controlName: string] : {[error: string]: string}};
    private defaultMessages: {[key: string]: string} = {
        required: ValidationMessage.required()
    };
    private groups: FormGroup[];
    private groupsWithName: Map<string, FormGroup> = new Map();
    private subscriptions: Map<string, Subscription>;
    private messageContainer: {[key: string]: string};

    constructor(){
        this.itemsToSkip = [];
        this.validationMessages = {
            
        };
        this.groups = [];
        this.subscriptions = new Map();
    }

    withGroup(group: FormGroup, groupName?: string): ValidationErrorBuilder{
        if(groupName){
            this.groupsWithName.set(groupName, group);
            return this;
        }
        this.groups.push(group);
        return this;
    }

    skip(keys: string[]){
        this.itemsToSkip = this.itemsToSkip.concat(keys);
        return this;
    }

    withMessages(messages:  {[controlName: string] : {[error: string]: string}}){
        this.validationMessages = {
            ...this.validationMessages,
            ...messages
        }
        return this;
    }

    withCustomValidationMessages(messages: {[key: string]: string}){        
        this.defaultMessages = {
            ...this.defaultMessages,
            ...messages
        }        
        return this;
    }
    
    useMessageContainer(container: {[key: string]: string}){
        this.messageContainer = container;
        return this;
    }   

    build(): Map<string, Subscription> {
        this.groups.forEach(group => {
            this.startWatching(group);
        });

        this.groupsWithName.forEach((group, groupName) => {
            this.startWatching(group, groupName);
        })
        return this.subscriptions;
    }

    private startWatching (group: FormGroup, groupName?: string){        
        Object.keys(group.controls)
        .forEach((controlName: string) => {
            if(!this.itemsToSkip.includes(controlName)){
                let key = groupName ? groupName + controlName : controlName;
                let control = group.get(controlName); 

                let sub = control.valueChanges
                .pipe(debounceTime(500))
                .subscribe(x => {
                    this.setValidationMsg(control, key);
                });                            
                this.subscriptions.set(key, sub);
            }
        });                       
    }

    private setValidationMsg(control: AbstractControl, controlName: string){
        this.messageContainer[controlName] = "";
        if((control.touched || control.dirty) && control.errors){
            this.messageContainer[controlName] = 
            Object.keys(control.errors).map(error => {                
                if(this.validationMessages[controlName]){
                    return this.validationMessages[controlName][error] || this.defaultMessages[error];
                }
                return this.defaultMessages[error];
            }).join(' ');
        }
    }
}