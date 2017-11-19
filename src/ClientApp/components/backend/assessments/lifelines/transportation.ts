import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Transportation } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    facilitiesLookUp: LookUp[] = [];
    model: Transportation = new Transportation();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setTransportationFormLookup',(barangays: LookUp[],facilities: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.facilitiesLookUp = facilities;
        })

        eventBus.$on('newTransportationAssessment', () => {
            this.showForm = true;
            this.model = new Transportation();
        })        

        eventBus.$on('editTransportationAssessment', (data: Transportation) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setTransportationFormLookup');
        eventBus.$off('newTransportationAssessment');
        eventBus.$off('editTransportationAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveTransportationAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
