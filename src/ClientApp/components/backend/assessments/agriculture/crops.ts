import Vue, { component } from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { Crops } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    cropsLookUp: LookUp[] = [];
    model: Crops = new Crops();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setCropsFormLookup',(barangays: LookUp[],crops: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.cropsLookUp = crops;
        })

        eventBus.$on('newCropsAssessment', () => {
            this.showForm = true;
            this.model = new Crops();
        })        

        eventBus.$on('editCropsAssessment', (data: Crops) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setCropsFormLookup');
        eventBus.$off('newCropsAssessment');
        eventBus.$off('editCropsAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveCropsAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
