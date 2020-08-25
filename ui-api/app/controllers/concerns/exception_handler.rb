module ExceptionHandler
  extend ActiveSupport::Concern

  included do
    rescue_from File::FileNotFound do |e|
      json_response({ message: e.message}, :not_found )
    end
  end
end