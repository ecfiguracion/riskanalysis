import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { WaterFacilities } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    facilitiesLookUp: LookUp[] = [];
    model: WaterFacilities = new WaterFacilities();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setWaterFacilitiesFormLookup',(barangays: LookUp[],facilities: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.facilitiesLookUp = facilities;
        })

        eventBus.$on('newWaterFacilitiesAssessment', () => {
            console.log("newWaterFacilitiesAssessment");
            this.showForm = true;
            this.model = new WaterFacilities();
        })        

        eventBus.$on('editWaterFacilitiesAssessment', (data: WaterFacilities) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setWaterFacilitiesFormLookup');
        eventBus.$off('newWaterFacilitiesAssessment');
        eventBus.$off('editWaterFacilitiesAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveWaterFacilitiesAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
