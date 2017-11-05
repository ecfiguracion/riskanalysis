import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { PagedBaseVM } from "../../../core/pagedbasevm";

@Component
export default class IndexComponent extends Vue {

    // Data Property
    vm: PagedBaseVM = new PagedBaseVM("api/lookups", true);

    // Life Cycle Hook
    mounted() {
        this.vm.onSearch();
    }

    // Component Methods
    onNew() {
        this.$router.push("/lookups/0");
    }

    onUpdate(id: number) {
        this.$router.push("/lookups/" + id.toString());
    }

    onDelete(id: number) {
        this.vm.onDelete(id)
            .then(data => {
                this.vm.onSearch();
            })
            .catch(error => { });
    }
}
