import  LookUp  from "../../model/lookup";

class Population {
    id: number = 0;
    entity: LookUp;
    barangay: LookUp;
    total: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Properties {
    id: number = 0;
    structure: LookUp;
    barangay: LookUp;
    totallyDamaged: number = 0;
    totallyDamagedUnit: string = '';
    criticallyDamaged: number = 0;
    criticallyDamagedUnit: string = '';
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Transportation {
    id: number = 0;
    description: string;
    facility: LookUp;
    barangay: LookUp;
    isPassable: boolean = false;
    lengthKM: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Communication {
    id: number = 0;
    facility: LookUp;
    barangay: LookUp;
    isOperational: boolean = false;
    total: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class ElectricalPower {
    id: number = 0;
    facility: LookUp;
    barangay: LookUp;
    isOperational: boolean = false;
    total: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class WaterFacilities {
    id: number = 0;
    facility: LookUp;
    barangay: LookUp;
    isOperational: boolean = false;
    total: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Crops {
    id: number = 0;
    barangay: LookUp;
    crops: LookUp;
    areaDamaged: number;
    metricTons: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Fisheries {
    id: number = 0;
    barangay: LookUp;
    fishery: LookUp;
    areaDamaged: number;
    metricTons: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Livestock {
    id: number = 0;
    assessmentId: number = 0;
    barangay: LookUp;
    livestock: LookUp;
    total: number;
    estimatedCost: number;
    rowId: number = 0;
    isdeleted: boolean = false;
}

class Assessment {
    /* One to One Properties */
    id: number = 0;
    typhoon: number = 0;
    remarks: string;

    /* One to Many Properties */
    population: Population[] = [];    
    properties: Properties[] = [];
    transportation: Transportation[] = [];
    communication: Communication[] = [];
    electricalPower: ElectricalPower[] = [];
    waterFacilities: WaterFacilities[] = [];
    crops: Crops[] = [];
    fisheries: Fisheries[] = [];
    livestocks: Livestock[] = [];
}

export { 
    Assessment, Population, Properties, Transportation, 
    Communication, ElectricalPower, WaterFacilities, Crops,
    Fisheries, Livestock
}