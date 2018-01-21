var server_protocol_domain = 'http://localhost:51113/';
var CONSTANTS = {
    SERVER_ROUTES: {
        GET_USER: server_protocol_domain + 'Users/GetUser',
        LOGIN_USER: server_protocol_domain + 'Users/LoginUser',
        REGISTER_USER: server_protocol_domain + 'Users/RegisterUser',
        GET_PORTFOLIO: server_protocol_domain + 'Portfolio/GetUserPortfolio',
        ADD_COIN: server_protocol_domain + 'Portfolio/AddCoin',
        REFRESH_PORTFOLIO: server_protocol_domain + 'Portfolio/RefreshPortfolio',
        DELETE_PORTFOLIO: server_protocol_domain + 'Portfolio/DeletePortfolio',
        DOWNLOAD_PORTFOLIO: server_protocol_domain + 'Portfolio/DownloadPortfolio'
    },
    GUID_EMPTY: '00000000-0000-0000-0000-000000000000'
};