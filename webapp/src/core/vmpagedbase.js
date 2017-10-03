import axios from 'axios'
import API from './constants/api'
import COMMON from './constants/common'

var NAVIGATION_TYPE = {
    NEXT: 1,
    PREVIOUS: 2,
    FIRST: 3,
    LAST:4
}

class VMPagedBase {
    constructor () {        

        // Handle the model data
        this.pagedmodel = {}

        // Handle data filter
        this.Filters = { Search: { Text:'' }, SelectedIds: []}            

        // Required flags to be set 
        this.IsUseToken = false
        this.UrlEndPoint = ''
    }

    getUrl (endpoint) {
        if (this.IsUseToken)
            return API.KEYS.URL + this.UrlEndPoint + endpoint + API.KEYS.TOKEN_REQUEST
        else
            return API.KEYS.URL + this.UrlEndPoint + endpoint
    }

    refresh() {
        return this.submit(this.getUrl('/search'),COMMON.ACTION_TYPES.SEARCH)
    }

    delete() {
        return this.submit(this.getUrl('/delete'),COMMON.ACTION_TYPES.DELETE)
    }

    submit (url, action) {
        return new Promise((resolve, reject) => {
            
            var data = []            
            var requestType = 'get'

            if (action == COMMON.ACTION_TYPES.SEARCH) {
                data = this.Filters.Search
                requestType = 'post'                    
            }
            else if (action == COMMON.ACTION_TYPES.DELETE) {
                data = this.Filters.SelectedIds     
                requestType = 'post'
            }    

            axios[requestType](url, data)
            .then(response => {
                if (action == COMMON.ACTION_TYPES.SEARCH) {
                    this.pagedmodel = response.data
                }
                resolve(response.data)
            })
            .catch(error => {
                reject(error.response)
            })
        })
    }

    goFirstPage () {
        this.goToPage(NAVIGATION_TYPE.FIRST)        
    }

    goNextPage () {
        this.goToPage(NAVIGATION_TYPE.NEXT)
    }

    goPreviousPage () {
        this.goToPage(NAVIGATION_TYPE.PREVIOUS)
    }    

    goLastPage () {
        this.goToPage(NAVIGATION_TYPE.LAST)
    }       

    goToPage (navigationType) {
        var urlPage = ''
        switch (navigationType) {
            case NAVIGATION_TYPE.FIRST:
                urlPage = 'first_page_url'
                break
            case NAVIGATION_TYPE.NEXT:
                urlPage = 'next_page_url'
                break
            case NAVIGATION_TYPE.PREVIOUS:
                urlPage = 'prev_page_url'
                break
            case NAVIGATION_TYPE.LAST:
                urlPage = 'last_page_url'
        }

        if (this.pagedmodel[urlPage] != null) {
            var url = this.pagedmodel[urlPage];
            if (this.IsUseToken) {
                url = url + '&api_token=' + API.KEYS.API_KEY
            }
            this.submit(url,COMMON.ACTION_TYPES.SEARCH)
        }
    }           
}

export default VMPagedBase
