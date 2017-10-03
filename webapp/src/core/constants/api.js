import auth from '../authenticate'

const KEYS = {
    URL:  process.env.API_URL,
    TOKEN_REQUEST: '?api_token='+ auth.getAPIKey(),
    API_KEY: auth.getAPIKey()
}

export default { KEYS }