import axios from "axios";

interface PagedParams {
    searchString: string;
    prevSearchString: string;
    pageNo: number;
    pageSize: number;
    pageCount: number;
}

export class PagedBaseVM {

    public APIUrl: string;
    public IsUseToken: boolean;
    public PagedParams: PagedParams;
    public Model: object[];

    constructor(apiUrl: string,isUseToken: boolean) {
        this.IsUseToken = isUseToken;
        this.APIUrl = apiUrl;
        this.PagedParams = { searchString: '', prevSearchString: '', pageNo: 1, pageSize: 10, pageCount: 0 };    
        this.Model = [];
    }

    onSearch() {
        axios.get(this.APIUrl, {
            params: this.PagedParams
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
            axios.delete(this.APIUrl + "/" + id.toString())
                .then(response => {
                    resolve(response);
                })
                .catch(error => {
                    reject(error.response)
                })
        });
    }
}
