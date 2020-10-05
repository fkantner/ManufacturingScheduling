class FileController < ApplicationController
  before_action :set_fr

  def count
    filter = params[:filter]
    @count = @fr.count(filter)
    json_response(@count)
  end

  def get
    filename = params[:filename]
    @json = @fr.get(filename)
    json_response(@json.to_json)
  end

  def list
    filter = params[:filter]
    @json = @fr.list(filter)
    json_response(@json)
  end

  def list_types
    @json = @fr.list_types
    json_response(@json)
  end

  private

  def set_fr
    @fr = FileReader.new
  end
end