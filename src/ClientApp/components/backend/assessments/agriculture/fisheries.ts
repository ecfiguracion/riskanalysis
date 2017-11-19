import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Fisheries } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    fisheriesLookUp: LookUp[] = [];
    model: Fisheries = new Fisheries();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setFisheriesFormLookup',(barangays: LookUp[],facilities: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.fisheriesLookUp = facilities;
        })

        eventBus.$on('newFisheriesAssessment', () => {
            this.showForm = true;
            this.model = new Fisheries();
        })        

        eventBus.$on('editFisheriesAssessment', (data: Fisheries) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setFisheriesFormLookup');
        eventBus.$off('newFisheriesAssessment');
        eventBus.$off('editFisheriesAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveFisheriesAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
