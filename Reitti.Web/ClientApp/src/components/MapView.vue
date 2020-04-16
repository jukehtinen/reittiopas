<template>
  <div class="panzoom-parent mt-3">
    <Map class="map-view"></Map>
  </div>
</template>

<script>
import Map from "./Map";
import panzoom from "panzoom";

export default {
  name: "MapView",
  components: { Map },
  props: ["route"],

  mounted() {
    panzoom(document.querySelector("#g5082"), {
      bounds: true,
      boundsPadding: 0.1,
      maxZoom: 1,
      minZoom: 0.1
    });
  },

  watch: {
    route: function(newRoute, oldRoute) {
      if (oldRoute) {
        oldRoute.stops.forEach(s => {
          document.getElementById(`asema-${s.stop1}`).style = "";
          document.getElementById(`asema-${s.stop2}`).style = "";
        });
      }
      newRoute.stops.forEach(s => {
        document.getElementById(`asema-${s.stop1}`).style = "fill: #a26d3f;";
        document.getElementById(`asema-${s.stop2}`).style = "fill: #a26d3f;";
      });
      document.getElementById(`asema-${newRoute.from}`).style = "fill: #008b8b;";
      document.getElementById(`asema-${newRoute.to}`).style = "fill: #3388de;";
    }
  }
};
</script>

<style>
.panzoom-parent {
  position: relative;
  border: 2px solid #ced4da;
  border-radius: 5px;
  overflow: hidden;
  background-color: #b9b9b9;
}
.map-view {
  width: 100%;
  height: 100%;
  outline: none;
}
</style>
