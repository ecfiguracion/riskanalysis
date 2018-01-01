import axios from "axios";
import { store } from "../boot";

interface PagedParams {
    searchString: string;
    prevSearchString: string;
    pageNo: number;
    pageSize: number;
    pageCount: number;
    parameter1?: number;
    parameter2?: number;
    parameter3?: number;
    parameter5?: number;
}

export class PagedBaseVM {

    public APIUrl: string;
    public IsUseToken: boolean;
    public PagedParams: PagedParams;
    public ExtParams: any[];
    public Model: object[];

    constructor(apiUrl: string,isUseToken: boolean) {
        this.IsUseToken = isUseToken;
        this.APIUrl = apiUrl;
        this.PagedParams = { searchString: '', prevSearchString: '', pageNo: 1, pageSize: 8, pageCount: 0 };    
        this.Model = [];
        this.ExtParams = [];
    }

    onSearch() {
        //var paramValues: object[] = [];
        //paramValues.push(this.PagedParams);
        //paramValues.push(this.ExtParams);

        axios.get(this.APIUrl, {
            params: this.PagedParams,
            headers: { tokenAuthorization : store.getters.token }
        })
        .then(response => {
            this.Model = response.data.items;
            this.PagedParams = response.data.pagedInfo as PagedParams;
            this.PagedParams.prevSearchString = this.PagedParams.searchString;
        })
    }

    onMoveFirst() {
        if (this.PagedParams.pageNo != 1) {
            this.PagedParams.pageNo = 1;
            this.onSearch();
        }
    }

    onMoveNext() {
        if (this.PagedParams.pageNo < this.PagedParams.pageCount) {
            this.PagedParams.pageNo += 1;
            this.onSearch();
        }
    }

    onMovePrevious() {
        if (this.PagedParams.pageNo > 1) {
            this.PagedParams.pageNo -= 1;
            this.onSearch();
        }
    }

    onMoveLast() {
        if (this.PagedParams.pageNo != this.PagedParams.pageCount) {
            this.PagedParams.pageNo = this.PagedParams.pageCount;
            this.onSearch();
        }
    }

    onDelete(id: number) {
        return new Promise((resolve, reject) => {
            axios.delete(this.APIUrl + "/" + id.toString(), {
                    headers: { tokenAuthorization: store.getters.token }
                })
                .then(response => {
                    resolve(response);
                })
                .catch(error => {
                    reject(error.response)
                })
        });
    }
}
