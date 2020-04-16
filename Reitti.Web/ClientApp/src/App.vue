<template>
  <div id="app">
    <b-container>
      <b-navbar variant="primary" type="dark">
        <b-navbar-brand>Reittiopas</b-navbar-brand>
        <b-spinner v-if="isLoading" variant="primary" label="Loading"></b-spinner>
      </b-navbar>

      <b-row v-if="!stops">
        <b-col class="mt-3">
          <b-alert show variant="danger">Error! Try reloading the page.</b-alert>
        </b-col>
      </b-row>

      <b-row>
        <b-col>
          <b-form>
            <b-form-select v-model="stopFrom" :options="stops" size="sm" class="mt-3">
              <template v-slot:first>
                <b-form-select-option :value="undefined" disabled>Starting stop</b-form-select-option>
              </template>
            </b-form-select>
            <b-form-select v-model="stopTo" :options="stops" size="sm" class="mt-3">
              <template v-slot:first>
                <b-form-select-option :value="undefined" disabled>Target stop</b-form-select-option>
              </template>
            </b-form-select>
          </b-form>
          <b-card class="mt-3" :header="routeCardTitle" v-if="route">
            <div class="routeItem" v-for="stop in route.stops" :key="stop.stop1">
              <strong>{{ stop.stop1 }}</strong>
              <b-icon icon="arrow-right"></b-icon>
              <strong>{{ stop.stop2 }}</strong>
              <div class="align-middle">
                {{ stop.time }}
                <b-icon icon="clock"></b-icon>
              </div>
              <div style="width: 100px;">
                <b-icon
                  class="px+5"
                  icon="square-fill"
                  v-bind:style="{color: getLineColor(stop.line)}"
                ></b-icon>
                {{ stop.line }}
              </div>
            </div>
          </b-card>
        </b-col>
      </b-row>

      <b-row>
        <b-col>
          <MapView :route="route"></MapView>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>

<script>
import MapView from "./components/MapView";
import { api } from "./api";

export default {
  name: "App",
  components: { MapView },

  data: function() {
    return {
      isLoading: true,
      stops: [],
      stopFrom: undefined,
      stopTo: undefined,
      route: undefined,
      routeCardTitle: ""
    };
  },

  async created() {
    await this.getStops();
  },

  methods: {
    async getStops() {
      this.isLoading = true;
      this.stops = [];
      this.stops = await api.getStops();
      this.isLoading = false;
    },

    async getRoute() {
      if (!this.stopFrom || !this.stopTo) {
        return;
      }
      this.isLoading = true;
      this.route = await api.getRoute(this.stopFrom, this.stopTo);
      this.routeCardTitle = `Route ${this.route.from} to ${this.route.to} (total time ${this.route.totalTime})`;
      this.isLoading = false;
    },

    getLineColor(line) {
      const lineClasses = {
        keltainen: "#f3a833",
        punainen: "#ec273f",
        vihreä: "#5ab552",
        sininen: "#3388de"
      };
      return lineClasses[line];
    }
  },

  watch: {
    stopFrom: function() {
      this.getRoute();
    },
    stopTo: function() {
      this.getRoute();
    }
  }
};
</script>

<style>
.routeItem {
  display: grid;
  grid-template-columns: 30px 50px 30px 80px auto;
  justify-items: center;
  align-items: center;
}
</style>
