import Vue, { component } from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Livestock } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    livestockLookUp: LookUp[] = [];
    model: Livestock = new Livestock();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setLivestockFormLookup',(barangays: LookUp[],livestock: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.livestockLookUp = livestock;
        })

        eventBus.$on('newLivestockAssessment', () => {
            this.showForm = true;
            this.model = new Livestock();
        })        

        eventBus.$on('editLivestockAssessment', (data: Livestock) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setLivestockFormLookup');
        eventBus.$off('newLivestockAssessment');
        eventBus.$off('editLivestockAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveLivestockAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
