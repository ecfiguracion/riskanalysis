import Vue, { component } from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Population } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    populationsLookUp: LookUp[] = [];
    model: Population = new Population();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setPopulationFormLookup',(barangays: LookUp[],populations: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.populationsLookUp = populations;
        })

        eventBus.$on('newPopulationAssessment', () => {
            this.showForm = true;
            this.model = new Population();
        })        

        eventBus.$on('editPopulationAssessment', (data: Population) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setPopulationFormLookup');
        eventBus.$off('newPopulationAssessment');
        eventBus.$off('editPopulationAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('savePopulationAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
