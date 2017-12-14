class RiskMapSummary {
    population: string = "";
    properties: string = "";
    lifelines: string = "";
    agriculture: string = "";
    total: string = "";
}

class RiskMaps {
    sectionId: number;
    barangay: string;    
    summary: string;
    latitude: number;
    longitude: number;
}

class Charts {
    sectionId: number;
    name: string;    
    total: number;
}

class PopulationCommon {
    entityId: number;
    entityName: string;
    barangay: string;
    total: number;
}

class DamagedProperties {
    entityId: number;
    entityName: string;
    barangay: string;
    totallyDamaged: number;
    totallyDamagedUnit: string;
    criticallyDamaged: number;
    criticallyDamagedUnit: string
    estimatedCost: number;    
}

class Transportation {
    entityId: number;
    entityName: string;
    description: string;
    barangay: string;
    isPassable: boolean;
    lengthKM: number;
    estimatedCost: number;
}

class LifelinesCommon {
    entityId: number;
    entityName: string;
    barangay: string;
    isOperational: boolean;
    total: number;
    estimatedCost: number;
}

class AgricultureCommon {
    entityId: number;
    entityName: string;
    barangay: string;
    areaDamaged: number;
    metrictons: number;
    estimatedCost: number;    
}

class Livestock {
    entityId: number;
    entityName: string;
    barangay: string;
    total: number;
    estimatedCost: number;    
}

class HomeData {
    riskMapSummary: RiskMapSummary = new RiskMapSummary();
    riskMaps: RiskMaps[] = [];
    charts: Charts[] = [];
    displacedEvacuated: PopulationCommon[] = [];
    casualties: PopulationCommon[] = [];
    damagedProperties: DamagedProperties[] = [];
    transportation: Transportation[] = [];
    communication: LifelinesCommon[] = [];
    electrical: LifelinesCommon[] = [];
    waterFacilities: LifelinesCommon[] = [];
    crops: AgricultureCommon[] = [];
    fisheries: AgricultureCommon[] = [];
    livestocks: Livestock[] = [];
}

class RiskTrendsSupport {
    levelId: number;
    barangay: string;
    supportPercentage: number;
}

class RiskTrendsRules {
    id: number;
    ruleXBarangay: string;
    ruleYBarangay: string;
    supportX: number;
    supportY: number;
    support: number;
    confidence: number;
    lift: number;
}

class RiskTrends {
    riskTrendsSupport: RiskTrendsSupport[] = [];
    riskTrendsRules: RiskTrendsRules[] = [];
    supportStart: number;
    supportEnd: number
}

export { RiskMapSummary, RiskMaps, HomeData, RiskTrends, RiskTrendsSupport }