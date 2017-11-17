import Vue, { component } from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";
import  LookUp  from "../model/lookup";
import { Assessment, Population, Properties, Transportation, Communication, ElectricalPower, 
    WaterFacilities, Crops, Fisheries, Livestock } from "./model/assessment";
import { eventBus } from "../../../boot";
import bootbox from 'bootbox';

@Component({
    components: {
        populationForm: require('./population/population.vue.html'),
        propertiesForm: require('./properties/properties.vue.html'),
        transportationForm: require('./lifelines/transportation.vue.html'),
        communicationForm: require('./lifelines/communication.vue.html'),
        electricalPowerForm: require('./lifelines/electricalpower.vue.html'),
        waterFacilitiesForm: require('./lifelines/waterfacilities.vue.html'),
        cropsForm: require('./agriculture/crops.vue.html'),
        fisheriesForm: require('./agriculture/fisheries.vue.html'),
        livestockForm: require('./agriculture/livestock.vue.html')
    }
})

export default class FormComponent extends Vue {

    //#region Data

    // Model Property
    model: Assessment = new Assessment();

    // LookUps
    typhoonsLookUp: LookUp[] = [];
    selectedTab: number = 0;   

    // Counters
    populationIdCounter: number = 1;
    propertiesIdCounter: number = 1;
    transportationIdCounter: number = 1;
    communicationIdCounter: number = 1;
    electricalPowerIdCounter: number = 1;
    waterFacilitiesIdCounter: number = 1;
    cropsIdCounter: number = 1;
    fisheriesIdCounter: number = 1;
    livestockIdCounter: number = 1;

    //#endregion

    // Life Cycle Hook
    mounted() {
        var id = this.$route.params.id;
        axios.get("api/assessments/" + id)
            .then(response => {
                this.model = response.data;
            })

        axios.get("api/assessments/datalookups")
            .then(response => {

                this.typhoonsLookUp = response.data.typhoons;
                var barangaysLookUp = response.data.barangays;
                var populationLookUp = response.data.populationLookup;
                var structuresLookUp = response.data.structuresLookup;
                var transportationLookup = response.data.transportationLookup;
                var communicationLookup = response.data.communicationLookup;
                var electricalPowerLookup  = response.data.electricalPowerLookup;
                var waterFacilitiesLookup = response.data.waterFacilitiesLookup;
                var cropsLookup = response.data.cropsLookup;
                var fisheriesLookup = response.data.fisheriesLookup;
                var livestockLookup = response.data.livestockLookup;

                eventBus.$emit('setPopulationFormLookup',barangaysLookUp,populationLookUp);
                eventBus.$emit('setPropertiesFormLookup',barangaysLookUp,structuresLookUp);
                eventBus.$emit('setTransportationFormLookup',barangaysLookUp,transportationLookup);
                eventBus.$emit('setCommunicationFormLookup',barangaysLookUp,communicationLookup);
                eventBus.$emit('setElectricalPowerFormLookup',barangaysLookUp,electricalPowerLookup);
                eventBus.$emit('setWaterFacilitiesFormLookup',barangaysLookUp,waterFacilitiesLookup);                
                eventBus.$emit('setCropsFormLookup',barangaysLookUp,cropsLookup);
                eventBus.$emit('setFisheriesFormLookup',barangaysLookUp,fisheriesLookup);
                eventBus.$emit('setLivestockFormLookup',barangaysLookUp,livestockLookup);
            })

        // Events for assessments    
        eventBus.$on('savePopulationAssessment',(data: Population) => {
            if (data.rowId == 0) { 
                data.rowId = this.populationIdCounter;
                this.populationIdCounter++;                
                this.model.population.push(data);
            } else {
                this.$forceUpdate();
            }
        })          
        
        eventBus.$on('savePropertiesAssessment',(data: Properties) => {
            if (data.rowId == 0) { 
                data.rowId = this.propertiesIdCounter;
                this.propertiesIdCounter++;                
                this.model.properties.push(data);
            } else {
                this.$forceUpdate();
            }
        })    
        
        eventBus.$on('saveTransportationAssessment',(data: Transportation) => {
            if (data.rowId == 0) { 
                data.rowId = this.transportationIdCounter;
                this.transportationIdCounter++;                
                this.model.transportation.push(data);
            } else {
                this.$forceUpdate();
            }
        })     
        
        eventBus.$on('saveCommunicationAssessment',(data: Communication) => {
            if (data.rowId == 0) { 
                data.rowId = this.communicationIdCounter;
                this.communicationIdCounter++;                
                this.model.communication.push(data);
            } else {
                this.$forceUpdate();
            }
        })          

        eventBus.$on('saveElectricalPowerAssessment',(data: ElectricalPower) => {
            if (data.rowId == 0) { 
                data.rowId = this.electricalPowerIdCounter;
                this.electricalPowerIdCounter++;                
                this.model.electricalPower.push(data);
            } else {
                this.$forceUpdate();
            }
        })   
        
        eventBus.$on('saveWaterFacilitiesAssessment',(data: WaterFacilities) => {
            if (data.rowId == 0) { 
                data.rowId = this.waterFacilitiesIdCounter;
                this.waterFacilitiesIdCounter++;                
                this.model.waterFacilities.push(data);
            } else {
                this.$forceUpdate();
            }
        })        
        
        eventBus.$on('saveCropsAssessment',(data: Crops) => {
            if (data.rowId == 0) { 
                data.rowId = this.cropsIdCounter;
                this.cropsIdCounter++;                
                this.model.crops.push(data);
            } else {
                this.$forceUpdate();
            }
        })        
        
        eventBus.$on('saveFisheriesAssessment',(data: Fisheries) => {
            if (data.rowId == 0) { 
                data.rowId = this.fisheriesIdCounter;
                this.fisheriesIdCounter++;                
                this.model.fisheries.push(data);
            } else {
                this.$forceUpdate();
            }
        })   
        
        eventBus.$on('saveLivestockAssessment',(data: Livestock) => {
            if (data.rowId == 0) { 
                data.rowId = this.livestockIdCounter;
                this.livestockIdCounter++;                
                this.model.livestocks.push(data);
            } else {
                this.$forceUpdate();
            }
        })            
    }

    // Component Methods
    onSave() {
        axios.post("api/assessments", this.model)
        .then(response => {
            this.model = response.data;
            bootbox.alert("Record successfully saved.");
        })
        .catch(error => {
            bootbox.alert("Error occurs during save.");
        })
    }   

    onCancel() {
        this.$router.go(-1);
    }    

    onSelectedTab(id: number) {
        this.selectedTab = id;
    }
    
    //#region Population Assessment
    addNewPopulation() {
        eventBus.$emit('newPopulationAssessment');
    }

    onEditPopulation(model: Population) {
        eventBus.$emit('editPopulationAssessment',model);
    }

    onRemovePopulation(model: Population) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                    this.model.population = this.model.population.filter(x => x.rowId != model.rowId);
                else
                    model.isdeleted = true;
                }    
            }
        });               
    }
    //#endregion

    //#region Properties Assessment

    addNewProperties() {
        eventBus.$emit('newPropertiesAssessment');
    }

    onEditProperties(model: Properties) {
        eventBus.$emit('editPropertiesAssessment',model);
    }

    onRemoveProperties(model: Properties) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.properties = this.model.properties.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }    
            }
        });           
    }    
    //#endregion

    //#region Transportation Assessment

    addNewTransportation() {
        eventBus.$emit('newTransportationAssessment');
    }

    onEditTransportation(model: Transportation) {
        eventBus.$emit('editTransportationAssessment',model);
    }

    onRemoveTransportation(model: Transportation) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.transportation = this.model.transportation.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }    
            }
        });          
    }    

    //#endregion    

    //#region Communication Assessment

    addNewCommunication() {
        eventBus.$emit('newCommunicationAssessment');
    }

    onEditCommunication(model: Communication) {
        eventBus.$emit('editCommunicationAssessment',model);
    }

    onRemoveCommunication(model: Communication) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.communication = this.model.communication.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }   
            }
        });                
    }    

    //#endregion   

    //#region Electrical Power Assessment

    addNewElectricalPower() {
        eventBus.$emit('newElectricalPowerAssessment');
    }

    onEditElectricalPower(model: ElectricalPower) {
        eventBus.$emit('editElectricalPowerAssessment',model);
    }

    onRemoveElectricalPower(model: ElectricalPower) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.electricalPower = this.model.electricalPower.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                } 
            }
        });         
    }    

    //#endregion   
    
    //#region Water Facilities Assessment

    addNewWaterFacilities() {
        eventBus.$emit('newWaterFacilitiesAssessment');
    }

    onEditWaterFacilities(model: WaterFacilities) {
        eventBus.$emit('editWaterFacilitiesAssessment',model);
    }

    onRemoveWaterFacilities(model: WaterFacilities) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.waterFacilities = this.model.waterFacilities.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }
            }
        });           
    }    

    //#endregion   
    
    //#region Crops Assessment

    addNewCrops() {
        eventBus.$emit('newCropsAssessment');
    }

    onEditCrops(model: Crops) {
        eventBus.$emit('editCropsAssessment',model);
    }

    onRemoveCrops(model: Crops) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.crops = this.model.crops.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }
            }
        });          
    }    

    //#endregion      
    
    //#region Fisheries Assessment

    addNewFisheries() {
        eventBus.$emit('newFisheriesAssessment');
    }

    onEditFisheries(model: Fisheries) {
        eventBus.$emit('editFisheriesAssessment',model);
    }

    onRemoveFisheries(model: Fisheries) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.fisheries = this.model.fisheries.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }
            }
        });          
    }    

    //#endregion     

    //#region Livestock Assessment

    addNewLivestock() {
        eventBus.$emit('newLivestockAssessment');
    }

    onEditLivestock(model: Livestock) {
        eventBus.$emit('editLivestockAssessment',model);
    }

    onRemoveLivestock(model: Livestock) {
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    if (model.id == 0)
                        this.model.livestocks = this.model.livestocks.filter(x => x.rowId != model.rowId);
                    else
                        model.isdeleted = true; 
                }
            }
        });                  
    }    

    //#endregion      
    
}
