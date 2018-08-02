import { BaseModel } from './BaseModel';

export class QualityControl extends BaseModel{

    serial: string;
    product : string;
    productDescription: string;
    defect: string;
    defectDescrition:string;
    desicion: string;
    status: string;

    createDate:string;
    userCreated:string;
    lastModificationUser:string;
    lastModificationDate:string;
}