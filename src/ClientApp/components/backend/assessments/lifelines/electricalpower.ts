import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { eventBus } from "../../../../boot";
import  LookUp  from "../../model/lookup";
import { ElectricalPower } from "../model/assessment";

@Component
export default class FormComponent extends Vue {
    
    // Data Property
    barangaysLookUp: LookUp[] = [];
    facilitiesLookUp: LookUp[] = [];
    model: ElectricalPower = new ElectricalPower();

    showForm: boolean = false;

    // Life Cycle Hook
    mounted() {
    }

    created() {
        eventBus.$on('setElectricalPowerFormLookup',(barangays: LookUp[],facilities: LookUp[]) => {
            this.barangaysLookUp = barangays;
            this.facilitiesLookUp = facilities;
        })

        eventBus.$on('newElectricalPowerAssessment', () => {
            this.showForm = true;
            this.model = new ElectricalPower();
        })        

        eventBus.$on('editElectricalPowerAssessment', (data: ElectricalPower) => {
            this.showForm = true;
            this.model = data;
        })        
        
    }

    beforeDestroy() {
        eventBus.$off('setElectricalPowerFormLookup');
        eventBus.$off('newElectricalPowerAssessment');
        eventBus.$off('editElectricalPowerAssessment');
    }

    // Component Methods

    save() {
        this.showForm = false;
        eventBus.$emit('saveElectricalPowerAssessment',this.model);
    }

    close() {
        this.showForm = false;
    }
}
