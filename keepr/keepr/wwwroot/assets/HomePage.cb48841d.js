import{D as c,c as n,a as _,o as s,b as o,d as p,F as d,A as l,E as i}from"./vendor.b8f97afc.js";import{_ as m,k as u,P as f,A as k}from"./index.af39ecc8.js";const v={setup(){return c(async()=>{try{await u.getAll()}catch(e){console.error("[COULD_NOT_LOAD_KEEPS]",e.message),f.toast(e.message,"error")}}),{keeps:n(()=>k.keeps)}}},g={class:"container-fluid"},y={class:"masonry-with-columns"};function h(e,x,A,t,P,B){const r=_("Keep");return s(),o("div",g,[p("div",y,[(s(!0),o(d,null,l(t.keeps,a=>(s(),i(r,{key:a.id,keep:a},null,8,["keep"]))),128))])])}var D=m(v,[["render",h],["__scopeId","data-v-24c5b49c"]]);export{D as default};