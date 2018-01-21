'use strict';
let PortfolioView = {
    template: `
    <v-container grid-list-md text-xs-center>
        <v-layout row wrap>
            <v-flex xs12>
                <v-card class="green lighten-4">
                    <v-card-text class="px-0"><h2><b>Total amount: {{totalAmount}} USD</b></h2></v-card-text>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row wrap v-for="p in portfolio" :key="p.Id">
            <v-flex xs12>
                <v-card class="grey lighten-4">
                    <v-card-text class="px-0"><h2><b>{{p.Name}} - Amount: {{p.Amount}} Price: {{p.Price_USD}} USD</b></h2></v-card-text>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row justify-center>
            <v-btn dark fab color="blue" center slot="activator" v-on:click="selectCoinDialog=true">
                <v-icon>add</v-icon>
            </v-btn>
            <v-btn dark fab color="blue" center v-on:click="deletePortfolio()">
                <v-icon>delete</v-icon>
            </v-btn>
            <v-btn dark fab color="blue" center v-on:click="refreshPortfolio()">
                <v-icon>refresh</v-icon>
            </v-btn>
            <v-btn dark fab color="blue" center v-on:click="downloadPortfolio()">
                <v-icon>get_app</v-icon>
            </v-btn>
            <v-dialog v-model="selectCoinDialog" scrollable max-width="600px">                
                <v-card>
                    <v-card-title>Select Coin</v-card-title>
                    <v-divider></v-divider>
                    <v-card-text style="height: 500px;">
                        <v-text-field label="Filter" single-line v-model="filter"></v-text-field>
                        <div style="cursor:pointer;" v-on:click="addCoin(coin)" v-for="coin in coins" :key="coin.id">
                            <v-divider></v-divider>
                            <h2><b>{{coin.rank}}. {{coin.name}}</b></h2>
                            <v-divider></v-divider>
                        </div>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <v-dialog v-model="addCoinDialog" scrollable max-width="600px">
                <v-card>
                    <v-card-title><h2><b>{{selectedCoin.name}}</b></h2></v-card-title>
                    <v-divider></v-divider>
                    <v-card-text style="height: 500px;">
                        <v-form v-model="valid" ref="form">
                            <v-text-field label="Amount" v-model="selectedCoin.amount" :rules="amountRules" required></v-text-field>
                            <v-btn @click="add()" :disabled="!valid" color="primary" white--text><b>Add</b></v-btn>
                            <v-btn @click="selectCoin()">Go back</v-btn>
                        </v-form>
                    </v-card-text>
                </v-card>
            </v-dialog>
        </v-layout>
    </v-container>`,
    data() {
        return {
            totalAmount: 0,
            allCoins: [],
            coins: [],
            selectCoinDialog: false,
            addCoinDialog: false,
            filter: '',
            selectedCoin: {},
            valid: true,
            amountRules: [
                (v) => !!v || 'Amount is required'
            ],
            portfolio: []
        }
    },
    mounted(){
        if(UsersTable.userLoggedIn()){
            this.getPortfolio();
    
            this.$http.get('https://api.coinmarketcap.com/v1/ticker/?start=0&limit=1400').then(
                (data) => {
                    this.allCoins = data.body;
                    this.coins = this.allCoins;
                },
                (err) => {
                    console.log(err);
                }
            );
        }
        else {
            this.$router.push('/');
        }
    },
    watch: {
        filter: function(newFilter) {
            this.filterCollection();
        },
        addCoinDialog: function(newValue){
            if(!newValue){
                this.selectedCoin = {};
            }
        }
    },
    methods: {
        filterCollection(){
            this.coins = _.filter(this.allCoins, (el) => {
                return _.includes(el.name.toLowerCase(), this.filter.toLowerCase());
            });
        },
        addCoin(coin) {
            this.selectedCoin = coin;
            delete this.selectedCoin.amount;
            this.selectCoinDialog = false;
            this.addCoinDialog = true;
        },
        selectCoin() {
            this.selectCoinDialog = true;
            this.addCoinDialog = false;
        },
        add() {
            if (this.$refs.form.validate()) {     
                debugger;
                this.$http.post(CONSTANTS.SERVER_ROUTES.ADD_COIN, {
                    coin: {
                        UserEmail: UsersTable.getLoggedUserMail(),
                        Coin: {
                            Id: this.selectedCoin.id,
                            Name: this.selectedCoin.name,
                            Symbol: this.selectedCoin.symbol,
                            Rank: this.selectedCoin.rank,
                            Price_USD: this.selectedCoin.price_usd,
                            Amount: this.selectedCoin.amount
                        }
                    }
                }).then(function success(data) {
                    if (data.body.success) {
                        this.$router.push('/login');
                    }
                    console.log(data);
                },
                    function error(data) {
                        console.log(data);
                    });
                this.addCoinDialog = false;
            }
        },
        getPortfolio() {  
            debugger;
            this.$http.get(CONSTANTS.SERVER_ROUTES.GET_PORTFOLIO + '?email=' + UsersTable.getLoggedUserMail())
                .then(function success(data) {
                    debugger;
                    this.portfolio = data.body.result;
                    this.portfolio = this.portfolio ? this.portfolio : [];
                    if (this.portfolio && this.portfolio.length > 0) {
                        this.totalAmount = 0;
                        for (let i = 0; i < this.portfolio.length; i++) {
                            let currCoinAmount = this.portfolio[i].Amount;
                            this.totalAmount += parseFloat(currCoinAmount) * parseFloat(this.portfolio[i].Price_USD);
                        }
                        this.totalAmount = this.totalAmount.toFixed(2);
                    }
                    else {
                        this.totalAmount = 0;
                    }
                }, function error(data) {
                    console.log(data);
                });            
        },
        refreshPortfolio() {
            this.$http.get(CONSTANTS.SERVER_ROUTES.REFRESH_PORTFOLIO + '?email=' + UsersTable.getLoggedUserMail());
        },
        deletePortfolio() {
            this.$http.get(CONSTANTS.SERVER_ROUTES.DELETE_PORTFOLIO + '?email=' + UsersTable.getLoggedUserMail());
        },
        downloadPortfolio() {
            window.open(CONSTANTS.SERVER_ROUTES.DOWNLOAD_PORTFOLIO + '?email=' + UsersTable.getLoggedUserMail(), '_blank');
        }
    }
};