import Vue, { component } from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Properties } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    structuresLookUp: LookUp[] = [];
    model: Properties = new Properties();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setPropertiesFormLookup',(barangays: LookUp[],structures: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.structuresLookUp = structures;
        })

        eventBus.$on('newPropertiesAssessment', () => {
            this.showForm = true;
            this.model = new Properties();
        })        

        eventBus.$on('editPropertiesAssessment', (data: Properties) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setPropertiesFormLookup');
        eventBus.$off('newPropertiesAssessment');
        eventBus.$off('editPropertiesAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('savePropertiesAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
