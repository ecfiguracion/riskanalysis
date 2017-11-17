import Vue, { component } from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Communication } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    facilitiesLookUp: LookUp[] = [];
    model: Communication = new Communication();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setCommunicationFormLookup',(barangays: LookUp[],facilities: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.facilitiesLookUp = facilities;
        })

        eventBus.$on('newCommunicationAssessment', () => {
            this.showForm = true;
            this.model = new Communication();
        })        

        eventBus.$on('editCommunicationAssessment', (data: Communication) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setCommunicationFormLookup');
        eventBus.$off('newCommunicationAssessment');
        eventBus.$off('editCommunicationAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveCommunicationAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
