require 'json'

class FileReader
  FOLDER = Rails.root + "public/data"

  def count(test)
    return list(test).count
  end

  def get(filename)
    extendedFilename = filename + ".json"
    return JSON.parse(File.read(FOLDER + extendedFilename))
  end

  def list(test)
    return FOLDER.children.map{|x| File.basename(x)}.select{|x| x.include?(test)}
  end
end