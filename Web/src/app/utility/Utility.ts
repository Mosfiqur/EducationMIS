export class Utility {
    public static range(start: number, end: number, step?: number): number[]{
        let arr : number[]= [];

        step = step || 1;
        for(let i = start; i <= end; i+=step){
            arr.push(i);
        }
        return arr;
    }

    public static hasDuplicate(arr: number[]): boolean{
        let item = arr[0];
        for(let i = 1; i < arr.length; i++){
            if(arr[i] != item)
                return true;
        }
        return false;
    }
}