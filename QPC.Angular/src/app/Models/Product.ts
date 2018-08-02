import { BaseModel } from './BaseModel';

export class Product extends BaseModel{

    serial: string;
    product : string;
    productDescription: string;
    defect: string;
    defectDescrition:string;
    desicion: string;
    status: string;

}