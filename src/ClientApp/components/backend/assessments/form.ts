import Vue, { component } from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";
import  LookUp  from "../model/lookup";
import { Assessment, Population, Properties, Transportation, Communication, ElectricalPower, WaterFacilities } from "./model/assessment";
import { eventBus } from "../../../boot";
import bootbox from 'bootbox';

@Component({
    components: {
        populationForm: require('./population/population.vue.html'),
        propertiesForm: require('./properties/properties.vue.html'),
        transportationForm: require('./lifelines/transportation.vue.html')
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

    // Form Toggles
    showPopulationForm: boolean = false;
    showPropertiesForm: boolean = false;
    showTransportationForm: boolean = false;
    showCommunicationForm: boolean = false;
    showElectricalPowerForm: boolean = false;
    showWaterFacilitiesFomr: boolean = false;

    //#endregion

    // Life Cycle Hook
    mounted() {
        var id = Number(this.$route.params.id);
        //this.vm.find(id).then(data => { });

        axios.get("api/assessments/datalookups")
            .then(response => {

                this.typhoonsLookUp = response.data.typhoons;
                var barangaysLookUp = response.data.barangays;
                var populationLookUp = response.data.populationLookup;
                var structuresLookUp = response.data.structuresLookup;
                var transportationLookup = response.data.transportationLookup;

                eventBus.$emit('setPopulationFormLookup',barangaysLookUp,populationLookUp);
                eventBus.$emit('setPropertiesFormLookup',barangaysLookUp,structuresLookUp);
                eventBus.$emit('setTransportationFormLookup',barangaysLookUp,transportationLookup);
            })

        // Events for assessments    
        eventBus.$on('savePopulationAssessment',(data: Population) => {
            if (data.rowId == 0) { 
                data.rowId = this.populationIdCounter;
                this.populationIdCounter++;                
                this.model.population.push(data);
            }
        })          
        
        eventBus.$on('savePropertiesAssessment',(data: Properties) => {
            if (data.rowId == 0) { 
                data.rowId = this.propertiesIdCounter;
                this.propertiesIdCounter++;                
                this.model.properties.push(data);
            }
        })    
        
        eventBus.$on('saveTransportationAssessment',(data: Transportation) => {
            if (data.rowId == 0) { 
                data.rowId = this.transportationIdCounter;
                this.transportationIdCounter++;                
                this.model.transportation.push(data);
            }
        })         
    }

    // Component Methods
    onSave() {
        axios.post("api/assessments", this.model)
        .then(response => {
            console.log("everybody is happy");
        })
        .catch(error => {
            console.log("everybody is NOT happy");
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
        this.showPopulationForm = true;
        eventBus.$emit('newPopulationAssessment');
    }

    onEditPopulation(model: Population) {
        eventBus.$emit('editPopulationAssessment',model);
    }

    onRemovePopulation(model: Population) {
        bootbox.confirm('Are you sure you want to remove this record?',(result) => {
            if (result) {
                if (model.id == 0)
                    this.model.population = this.model.population.filter(x => x.rowId != model.rowId);
                else
                    model.isdeleted = true; 
            }
        });
    }
    //#endregion

    //#region Properties Assessment

    addNewProperties() {
        this.showPropertiesForm = true;
        eventBus.$emit('newPropertiesAssessment');
    }

    onEditProperties(model: Properties) {
        eventBus.$emit('editPropertiesAssessment',model);
    }

    onRemoveProperties(model: Properties) {
        bootbox.confirm('Are you sure you want to remove this record?',(result) => {
            if (result) {
                if (model.id == 0)
                    this.model.properties = this.model.properties.filter(x => x.rowId != model.rowId);
                else
                    model.isdeleted = true; 
            }
        });
    }    
    //#endregion

    //#region Transportation Assessment

    addNewTransportation() {
        this.showTransportationForm = true;
        eventBus.$emit('newTransportationAssessment');
    }

    onEditTransportation(model: Transportation) {
        eventBus.$emit('editTransportationAssessment',model);
    }

    onRemoveTransportation(model: Transportation) {
        bootbox.confirm('Are you sure you want to remove this record?',(result) => {
            if (result) {
                if (model.id == 0)
                    this.model.transportation = this.model.transportation.filter(x => x.rowId != model.rowId);
                else
                    model.isdeleted = true; 
            }
        });
    }    
    //#endregion    
}
