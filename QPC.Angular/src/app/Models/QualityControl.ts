import { BaseModel } from './BaseModel';

export class QualityControl extends BaseModel{

    product : string;
    defect: string;
    desicion: string;
    status: string;
}