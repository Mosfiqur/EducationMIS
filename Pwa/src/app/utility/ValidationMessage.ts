export class ValidationMessage {
    static invalidDateRange(): string {
        return `End date can't be before start date.`;        
    }
    static minlength(min: number): string{
        return `Should be at least ${min} characters`;        
    }
    static min(min: number): string{
        return `Should be greater than or equal to ${min}`;
    }
    static max(max: number): string{
        return `Should be less than or equal to ${max}`;
    }
    static maxlength(val: number): string{
        return `Maximum number of supported character is ${val}`;
    }
    static required(): string{
        return `This field is required`
    }
    static invlaidEmail(): string{
        return `Please enter a valid email address`
    }
    static duplicateNotAllowed(): string {
        return `Duplicate is not allowed.`
    }
}