import Vue from 'vue';
import { Component, Watch } from 'vue-property-decorator';
import axios from "axios";
import L from 'leaflet';
import Chart from 'chart.js';
import LookUp from '../backend/model/lookup';
import LookUpLink from '../backend/model/lookuplink';
import { RiskMapSummary, RiskMaps, HomeData } from './model/homemodel';
import { createDecorator } from 'vue-class-component/lib/util';
const marker_icon_population = require('../images/marker-icon-population.png');
const marker_icon_properties = require('../images/marker-icon-properties.png');
const marker_icon_lifelines = require('../images/marker-icon-lifelines.png');
const marker_icon_agriculture = require('../images/marker-icon-agriculture.png');
const marker_icon_shadow = require('../images/marker-shadow.png');

@Component
export default class HomeComponent extends Vue {

    /* Data Properties */
    typhoons: LookUp[] = [];
    sections: LookUp[] = [];
    categories: LookUpLink[] = [];

    /* Home Models */
    model: HomeData = new HomeData();

    map: L.Map;
    mapPopulationMarker: L.Marker[] = [];
    mapPropertiesMarker: L.Marker[] = [];
    mapLifelinesMarker: L.Marker[] = [];
    mapAgricultureMarker: L.Marker[] = [];
    selectedTyphoon: any = {};

    markerPopulationSelected: boolean = true;
    markerPropertiesSelected: boolean = false;
    markerLifelinesSelected: boolean = false;
    markerAgricultureSelected: boolean = false;
    
    /* Methods */
    createPopulationsChart() {
        
        var canvass : any = this.$refs.mapPopulations;
        var ctx = canvass.getContext("2d");

        var labels: string[] = [];
        var values: number[] = [];

        this.model.charts.forEach(item => {
            if (item.sectionId == 1) {
                labels.push(item.name);
                values.push(item.total);
            }
        });

        var myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Affected Populations',
                    data: values,
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

    createPropertiesChart() {
        var canvass : any = this.$refs.mapProperties;
        var ctx = canvass.getContext("2d");

        var labels: string[] = [];
        var values: number[] = [];

        this.model.charts.forEach(item => {
            if (item.sectionId == 2) {
                labels.push(item.name);
                values.push(item.total);
            }
        });

        var myChart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Damage Properties (In million)',
                    data: values,
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
    createLifelinesChart() {
        var canvass : any = this.$refs.mapLifelines;
        var ctx = canvass.getContext("2d");

        var labels: string[] = [];
        var values: number[] = [];

        this.model.charts.forEach(item => {
            if (item.sectionId == 3) {
                labels.push(item.name);
                values.push(item.total);
            }
        });

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Lifelines (In million)',
                    data: values,
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


    
    createAgricultureChart() {
        var canvass : any = this.$refs.mapAgriculture;
        var ctx = canvass.getContext("2d");


        var labels: string[] = [];
        var values: number[] = [];

        this.model.charts.forEach(item => {
            if (item.sectionId == 4) {
                labels.push(item.name);
                values.push(item.total);
            }
        });

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Damage on Agriculture (In million)',
                    data: values,
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
        var map = L.map('mapid',{
            center: [13.2354, 123.7781],
            zoom: 13
        })

        map.scrollWheelZoom.disable();
        
        map.on('focus', function() { map.scrollWheelZoom.enable(); });
        map.on('blur', function() { map.scrollWheelZoom.disable(); });
        
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox.streets',
            accessToken: 'pk.eyJ1IjoiZWNmaWd1cmFjaW9uIiwiYSI6ImNqNmNwZW5ybzF0cWwzM2w3YXh2enk2dXAifQ.HLzVBNEG5-czGJjBENEc-Q'
        }).addTo(map);

        this.map = map;
    }

    createMapLayer() {

        var customPopulationIcon = L.icon({
            iconUrl:  marker_icon_population.toString(),
            shadowUrl: marker_icon_shadow.toString()
        });

        var customPropertiesIcon = L.icon({
            iconUrl:  marker_icon_properties.toString(),
            shadowUrl: marker_icon_shadow.toString()
        });

        var customLifelinesIcon = L.icon({
            iconUrl:  marker_icon_lifelines.toString(),
            shadowUrl: marker_icon_shadow.toString()
        });
        
        var customAgricultureIcon = L.icon({
            iconUrl:  marker_icon_agriculture.toString(),
            shadowUrl: marker_icon_shadow.toString()
        });

        // Clear first map layer
        if (!this.markerPopulationSelected) {
            this.mapPopulationMarker.forEach(marker => {
                this.map.removeLayer(marker);
            });
        }

        if (!this.markerPropertiesSelected) {
            this.mapPropertiesMarker.forEach(marker => {
                this.map.removeLayer(marker);
            });
        }

        if (!this.markerLifelinesSelected) {
            this.mapLifelinesMarker.forEach(marker => {
                this.map.removeLayer(marker);
            });
        }

        if (!this.markerAgricultureSelected) {
            this.mapAgricultureMarker.forEach(marker => {
                this.map.removeLayer(marker);
            });
        }
        
        // Create marker
        this.model.riskMaps.forEach(item => {
            var createMarker:boolean = false;

            createMarker = (this.markerPopulationSelected && item.sectionId == 1) ||
                           (this.markerPropertiesSelected && item.sectionId == 2) ||            
                           (this.markerLifelinesSelected && item.sectionId == 3) ||            
                           (this.markerAgricultureSelected && item.sectionId == 4);

            if (createMarker) {
                var markerInfo = "<strong>" + item.barangay + "</strong><br/>" + item.summary;

                if (this.markerPopulationSelected && item.sectionId == 1) {  
                    var marker = L.marker([item.latitude, item.longitude], { icon: customPopulationIcon }).addTo(this.map)
                    .bindPopup(markerInfo);                                                         
                    this.mapPopulationMarker.push(marker);
                }
                if (this.markerPropertiesSelected && item.sectionId == 2) {
                    var marker = L.marker([item.latitude, item.longitude], { icon: customPropertiesIcon }).addTo(this.map)
                    .bindPopup(markerInfo);                                                                             
                    this.mapPropertiesMarker.push(marker);                 
                }   
                if (this.markerLifelinesSelected && item.sectionId == 3) {
                    var marker = L.marker([item.latitude, item.longitude], { icon: customLifelinesIcon }).addTo(this.map)
                    .bindPopup(markerInfo);                                                                                                 
                    this.mapLifelinesMarker.push(marker);                 
                }   
                if (this.markerAgricultureSelected && item.sectionId == 4) {
                    var marker = L.marker([item.latitude, item.longitude], { icon: customAgricultureIcon }).addTo(this.map)
                    .bindPopup(markerInfo);                                                                                                                     
                    this.mapAgricultureMarker.push(marker);
                }
            }
        });
    }

    clearMapLayer() {
        this.mapPopulationMarker.forEach(marker => {
            this.map.removeLayer(marker);
        });

        this.mapPropertiesMarker.forEach(marker => {
            this.map.removeLayer(marker);
        });        
        
        this.mapLifelinesMarker.forEach(marker => {
            this.map.removeLayer(marker);
        });        

        this.mapAgricultureMarker.forEach(marker => {
            this.map.removeLayer(marker);
        });        
    }

    formatNumber(value: number): string {
        var result = "";
        if (value > 1000000)
            result = Math.round(value / 1000000).toString() + "M";
        else if (value > 1000)
            result = Math.round(value / 1000).toString() + "K";
        else
            result = value.toString();
        return result;
    }

    mounted() {

        this.createMap();

        axios.get("api/home/datalookups")
        .then(response => {
            this.typhoons = response.data.typhoons;
            this.sections = response.data.sections;
            this.categories = response.data.categories;

            if (this.typhoons)
            {
                this.selectedTyphoon = this.typhoons[0];    
                this.markerPopulationSelected = true;
            }      
        });
    }

    @Watch('selectedTyphoon')
    onSelectedTyphoonPropertyChanged(value: LookUp, oldValue: LookUp) {
        axios.get("api/home/" + value.id.toString())
        .then(response => {
            
            this.model.riskMapSummary = response.data.riskMapSummary;
            this.model.riskMaps = response.data.riskMaps;
            this.model.charts = response.data.charts;
            this.model.displacedEvacuated = response.data.displacedEvacuated;
            this.model.casualties = response.data.casualties;
            this.model.damagedProperties = response.data.damagedProperties;
            this.model.transportation = response.data.transportation;
            this.model.communication = response.data.communication;
            this.model.electrical = response.data.electrical;
            this.model.waterFacilities = response.data.waterFacilities;
            this.model.crops = response.data.crops;
            this.model.fisheries = response.data.fisheries;
            this.model.livestocks = response.data.livestocks;
            
            this.clearMapLayer();

            this.createMapLayer();
            this.createPopulationsChart();
            this.createLifelinesChart();
            this.createPropertiesChart();
            this.createAgricultureChart();            
        });        
    }

    @Watch('markerPopulationSelected')
    markerPopulationSelectedPropertyChange(value: boolean, oldValue: boolean) {
        this.createMapLayer();
    }     
    
    @Watch('markerPropertiesSelected')
    markerPropertiesSelectedPropertyChange(value: boolean, oldValue: boolean) {
        this.createMapLayer();
    }     
    
    @Watch('markerLifelinesSelected')
    markerLifelinesSelectedPropertyChange(value: boolean, oldValue: boolean) {
        this.createMapLayer();
    }     

    @Watch('markerAgricultureSelected')
    markerAgricultureSelectedPropertyChange(value: boolean, oldValue: boolean) {
        this.createMapLayer();
    }        
}
