import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import L from 'leaflet';
import Chart from 'chart.js';

@Component
export default class HomeComponent extends Vue {

    createPopulationsChart() {
        
        var canvass : any = this.$refs.mapPopulations;
        var ctx = canvass.getContext("2d");

        var myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ["San Ramon", "San Roque", "Calayucay", "Nagsha", "Misericordia"],
                datasets: [{
                    label: 'Affected Populations',
                    data: [12, 100, 87,32,45],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1            
                }]
            }
        });        
    }

    createLifelinesChart() {
        var canvass : any = this.$refs.mapLifelines;
        var ctx = canvass.getContext("2d");

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Transportation", "Communication", "Electrical Power", "Water Facilities"],
                datasets: [{
                    label: 'Lifelines (In million)',
                    data: [3, 0, 0, 6],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            }
        });     
    }

    createPropertiesChart() {
        var canvass : any = this.$refs.mapProperties;
        var ctx = canvass.getContext("2d");

        var myChart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: ["Houses", "School Buildings", "Govt. Offices", "Public Markets", "Flood Control", "Commercial Facilities"],
                datasets: [{
                    label: 'Damage Properties (In million)',
                    data: [61.25, 3, 3, 1.5, 8, 5],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    xAxes: [{
                        stacked: true
                    }],
                    yAxes: [{
                        stacked: true
                    }]
                }
            }    
        });    
    }
    
    createAgricultureChart() {
        var canvass : any = this.$refs.mapAgriculture;
        var ctx = canvass.getContext("2d");

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Crops", "Fisheries", "Livestock", "Poultry and Fowls"],
                datasets: [{
                    label: 'Damage on Agriculture (In million)',
                    data: [100, 20, 30, 10],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            }
        });     
    }

    createMap() {
        var map = L.map('mapid').setView([13.2354, 123.7781], 16);   
        map.scrollWheelZoom.disable()
        
        map.on('focus', function() { map.scrollWheelZoom.enable(); });
        map.on('blur', function() { map.scrollWheelZoom.disable(); });
        
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox.streets',
            accessToken: 'pk.eyJ1IjoiZWNmaWd1cmFjaW9uIiwiYSI6ImNqNmNwZW5ybzF0cWwzM2w3YXh2enk2dXAifQ.HLzVBNEG5-czGJjBENEc-Q'
        }).addTo(map);
    }

    mounted() {
        this.createMap();
        this.createPopulationsChart();
        this.createLifelinesChart();
        this.createPropertiesChart();
        this.createAgricultureChart();
    }
}
