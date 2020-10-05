Rails.application.routes.draw do
  # For details on the DSL available within this file, see https://guides.rubyonrails.org/routing.html

  get '/types/', to: 'file#list_types'
  get '/file/:filename', to: 'file#get'
  get '/index/:filter', to: 'file#list'
  get '/count/:filter', to: 'file#count'
end
