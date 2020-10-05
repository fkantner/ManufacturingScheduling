module Response
  def json_response(object, status = :ok)
    logger.info object
    render json: object, status: status
  end
end