<template>
  <div class="container">

    <div class="row">
      <h1 class="col-6">Translate with chat GPT!</h1>
    </div>
    <div class="row">
      <div class="col-6">

        <button type="button" class="btn btn-primary btn-lg btn-block" data-toggle="dropdown" @click="toggle">Choose language to translate to</button>
        <div class="card float-window" v-if="active">
          <div class="card-body">      
            <button  type="button" class="btn btn-secondary" v-for="language in languages" @click="setLanguage(language)">{{ language }}</button>
          </div>
        </div>
      </div>
      <div class="col-5">
        <button type="submit" :disabled="input.trim()==='' || language.trim()===''" class="btn btn-primary btn-lg btn-block" @click="translate">Translate {{language != '' ?"to "+ language : '' }}</button>
      </div>
    </div>
    <div class="row">
      <div class="col-6">

        <textarea class="form-control" v-model="input" />
      </div>

      <div class="col-6">
        <textarea disabled class="form-control" :placeholder="translation"/>
      </div>

    </div>
  </div>
</template>

<script>
import axios from 'axios';
export default {
  name: 'Users',
  data() {
    return {
      input: '',
      translation: '',
      active: false,
      languages: ["Spanish", "English", "Ukrainian", "French"],
      language: ''

    }
  },

  methods: {
    toggle() {
      this.active = !this.active
    },
    setLanguage(language) {
      this.language = language
      this.toggle()
    },
    translate() {
      axios.post
        (
          'http://localhost:5014/Translate',
          {
            language: this.language,
            query: this.input
          }
        ).then(response => {
          console.log(response.data.result)
          this.translation = response.data.result
        })
    }
  }
}
</script>

<style scoped>
.float-window{
  position: absolute;
  z-index: 10;
}
.row{
  margin-top: 20px;
}
.btn{
  margin-bottom: 5px;
  display:block;
}
.form-control{
  height: 200px
}
</style>